
# Changelog #

## 1.8.3 ##

- Patch for v50
- Fix power malfunction showing lever jam tooltip incorrectly
- Fix soft lock when resetting objects after Malfunctions

## 1.8.2 ##

- Fix README.md typos because apparently I can't spell anymore

## 1.8.1 ##

- Add plan for next major upcoming release to README.md
- Fix network handlers registering twice
- Fix network handler MALFUNCTION_POWER not being called on host causing issues in singleplayer

## 1.8.0 ##

- Switch network library from LC_API to LethalNetworkAPI

## 1.7.1 ##

- Fix README.md containing two sections for the door malfunction

## 1.7.0 ##

- Add network syncing to make sure no desyncs happen due to penalty mechanic
- Add lever malfunction that will block take-off after a designated time until autopilot
- Decouple reliance on matched configuration files using host as source of truth

## 1.6.0 ##

- Add support for WeatherTweaks
- Add config option to allow consecutive malfunctions of the same type

## 1.5.6 ##

- Add banner image

## 1.5.5 ##

- Update plugin version identifier

## 1.5.4 ##

- Update README.md with new penalty and passed days mechanics

## 1.5.3 ##

- Fix #15: penalty desync by changing the order of operations and relying on the base game penalty calculation

## 1.5.2 ##

- Attempt to fix desync due to penalty calculating different amounts of bodies in the ship

## 1.5.1 ##

- Reduce elapsed days defaults by one day

## 1.5.0 ##

- Implement #12: Disable outdoor floodlights during power malfunction as per 
- Add penalty only option meaning malfunctions only happen when players are not recovered
- Add functionality to lock malfunctions behind a certain amount of elapsed days
- Tweak probabilities further

## 1.4.1 ##

- Fix #10: sparks were playing sound without spacialization meaning they could be heard everywhere
- Fix sparks collision depth buffer issue causing the players HUD to effect their collision (could cause sparks to get stuck in the players vision)
- Fix timed malfunctions triggering before the start of a mission if they were early in a day or past the previous level's take-off time of day

## 1.4.0 ##

- Make Power Malfunction have a chance at not being able to take-off until autopilot
- Fix Navigation Malfunction causing the ship to get stuck
- Fix Communication Malfunction not re-enabling the terminal
- Make teleporter during communication outage teleport a random player

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