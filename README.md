## Scenario

Punt37 is a Windows app that enables cross platform remote rebooting of a computer.

It has been designed to enable a Windows-based speech device to be restarted if something, anything, has gone wrong with the device.

The key scenario is that a person with disabilities that prevents verbal speech is using a Windows tablet with specialty software to
speak for them, perhaps with an Eye Tracking camera, and something has gone wrong.  Perhaps a driver has crashed or the software they
use to speak or emulate a mouse has stopped working.

Punt37 allows some external device -- such as a phone that can recognize facial gestures, a microcontroller with a physical switch, etc --
to send a 'force restart' command to the computer.

## Design Specification

For 'traditional' environments, such as business computers on a shared network with proper authentication, remote rebooting is built into Windows (see Shutdown.exe).  However for these cross-platform environments (such as calling from an Android phone), setting up the proper network call (NTLM, RPC, etc.) is a heavy programming burden.  So we're going to punt all that complicated authentication stuff in the name of 'how quickly can we get this working and tested?'

Instead, this app will:

- Listen on port 63737 for incoming HTTP calls
- Wait for an incoming HTTP call with custom verb "PUNT37" or an HTTP PUT with body "PUNT37"
- Force Reboot the computer

A remote app can call the equivalent of `curl testcomputer.local:63737` looking for an HTTP OK (e.g. 200) response to confirm that the target computer is running PUNT37 and ready to receive the reboot command.

### Test Process

- Add HTTP listening permissions by opening Command Prompt as Administrator and running `netsh http add urlacl url="http://+:63737/" user=everyone`
- Run Punt37.exe on the target computer (e.g. TestComputer)
- Future: Find the service on the network using DNS-SD
  - See: https://play.google.com/store/apps/details?id=com.druk.servicebrowser&hl=en_US
  - look for service name _punt37._tcp.
- ` curl -v testcomputer.local:63737`
  - Returns 200 if the app is running
  - Future: Returns 401 if the auth password is incorrect
- `curl -v -X PUNT37 testcomputer.local:63737`
  - Returns 200 if the reboot is successful, 500 if reboot failed
  - Future: Returns 401 if the auth password is incorrect
- `curl -v -X PUT -d "PUNT37" testcomputer.local:63737`
  - Returns 200 if the reboot is successful, 400 if a string other than PUNT37 is sent, 500 if reboot failed
  - Future: Returns 401 if the auth password is incorrect
  
### Features

- Runs as a System Tray icon

### Future Features

- Add DNS-SD support for publishing availability of PUNT37 for discovery
  - See https://github.com/anlam/DnsSDNet
- Add self-registration for auto-start on login
- Add self-registration for HTTP listening permissions
- Add HTTP basic auth and the ability to choose a password/token must be passed and implement HTTP 401 for GET and PUNT37 verbs.
