
# Changelog #

## 1.3.1 ##

- Fix the readme stating that the door malfunction opens at 11pm when in actuality it opens at 10pm

## 1.3.0 ##

- Add penalty mechanic
    + Not recovering a body multiplies the chance of encountering malfunctions
- Add Door Malfunction
    + Has a chance of happening after 4 hours and beyond into the round
    + Causes doors to remain closed with door controls disabled until 10pm
- Add Distortion Malfunction
    + Has a chance of happening immediately or after a random interval
    + Disables the map display, terminal and walkies

## 1.2.3 ##

- Fix #5: hangar door panel not reappearing after power malfunction ends 
- Add lighting, depth buffer physics and sounds to sparks effect

## 1.2.2 ##

- Make sparks look a little less like cheese balls
- Refactor internal asset loading

## 1.2.1 ##

- Adjust default probabilities
- Attempt fix for issue #2: Weather stuck on all maps after navigational malfunction

## 1.2.0 ##

- Add Power Malfunction
    + Notification plays while landing
    + Disables the map display, terminal, teleporters, charging station and door controls
- Add Teleporter Malfunction
    + Has a chance of happening immediately or after a random interval
    + Blocks players from using any ship teleporter
    + Notification is broadcast once malfunction takes effect
- Navigation Malfunction
    + Fix company building being a possible route
    + Fix notification playing when restarting a save

## 1.1.1 ##

- Add compatibility with Corporate Restructure (jamil.corporate_restructure)
- Fix desync issue by binding to map seed with added daily UTC epoch

## 1.1.0 ##

- Navigation Malfunction
    + Improve notification text
    + Improve navigation map text
    + Add sparks to lever while active
    + Block the user from choosing a different moon until malfunction ends

## 1.0.0 ##

- Initial release with basic functionality