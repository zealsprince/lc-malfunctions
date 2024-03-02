
<img src="https://img.shields.io/badge/version-1.8.2-0AF" /></a>
<img src="https://img.shields.io/badge/lc--version-v49-000" /></a>

![banner](https://github.com/zealsprince/lc-malfunctions/assets/1859270/f2d781e8-2b79-4d80-9e49-d688cc7b99f2)

# Malfunctions #

*The ship has experienced its fair share of crews and their eventual expiration. Even though it's stood for long, it's got some scratches of its own and they're showing more and more as the days go on.*

This mod adds random malfunctions to the ship and intends to improve the core game loop with interesting new metagame events.

## Events ##

### Route Table Offset / Navigation Malfunction ###

This malfunction appears after the ship goes back into orbit. It overwrites all routing commands and steers the ship to a random moon at no cost. No information about the moon or the weather is given to the players forcing them to possibly go on a tough expedition.

A warning is given after the ship enters orbit. The terminal routing will be blocked until the ship leaves the rerouted planet.

### Atomic Misalignment / Teleporter Malfunction ###

This malfunction causes teleporters to become temporarily disabled during an expedition.

It has a high chance of happening moments after landing but can also happen over the course of an expedition at a random moment. A warning after triggering is given to all players.

### Electromagnetic Disturbance / Communication Malfunction ###

This malfunction causes the communication and map systems to fail during an expedition. Teleporters are still functional but target players at random. Use with caution.

Ship terminal and map displays are either disabled immediately or after a random interval. Walkie talkies lose functionality as well. A warning after triggering is given to all players. 

### Crackdown Protocol / Door Malfunction ###

This malfunction causes the ship to go into a lockdown, causing the door to remain closed until 10pm. The door controls are disabled as well.

The malfunction triggers randomly over the course of a day if it's active. A warning after triggering is given to all players.

### Hydraulics Jam / Lever Malfunction ###

This malfunction causes the ship to broadcast a message stating the manual hydraulics are about to fail, blocking the lever at a certain time until the autopilot takes off.

The malfunction triggers randomly between 12pm and 4pm but gives players a 3 hour warning to decide whether they want to stay or take off before failure.

### Core Surge / Power Malfunction ###

This malfunction causes the ship to lose power to all its primary systems. This means batteries can not be recharged, doors can't be closed, the map can't be used, players can't be teleported and the terminal remains disabled. Furthermore, there might not be enough energy left to take off with only the emergency autopilot being the last option.

This is the worst type of malfunction.

To add onto that, no warning is given with this malfunction. The only solution to restore power is to use the remainder to launch back into orbit, possibly only through the autopilot reserves.

## Mechanics ##

Though the chance of malfunctions occurring is completely random, there are a few parameters that influence when they occur:

### Chances & Penalty ###

*The company likes its employees in one piece and while it does maintain your ship, incurring extra casualties is certainly removing it from that maintenance budget.*

In general the idea for malfunctions was that over the course of 2 in game weeks of each 4 days, there is about a 25% chance of a malfunction being encountered by default statistically speaking. This percentage has changed with the amount of additional malfunction mechanics I have added to this mod and as such overall default chances have been adjusted accordingly.

There is no weighted system in place meaning they are fully random. Due to it being pure randomness, you can have a run with back-to-back malfunctions. The likelihood of this happening are less than a percent though. Additionally, if a malfunction of a type was triggered it can not be triggered the next day.

To make malfunctions feel less random at times and more as a punishing mechanic, a penalty mechanic was added: Not recovering bodies by default doubles the chance of a malfunction happening. This is why by default the chances of a malfunction happening are very low.

**Keep in mind this is a bit of a spoiler if you're wanting to go in completely blind!**

<details> 
  <summary>Malfunction probabilities:</summary>

- Navigation: 7.5%
- Teleporter: 7.5%
- Distortion: 5.0%
- Door: 3.0%
- Lever: 3.0%
- Power: 1.5%
</details>

### Passed Days ###

Each malfunction has a configuration parameter to gatekeep them from happening early on in runs and as such feeling overwhelming.

**Once again, this might be a spoiler if you want to be surprised**

<details> 
  <summary>Malfunctions required passed days:</summary>

- Navigation: 3 (Week 2)
- Teleporter: 11 (Week 4)
- Distortion: 3 (Week 2)
- Door: 7 (Week 3)
- Lever: 0 (Week 1)
- Power: 11 (Week 4)
</details>

## Future Plans (2.0.0) ##

For the upcoming and next major version I will be focusing on counter-play to the malfunctions. This means players will be able to make more determined decisions when dealing with malfunctions and remediate them. The following major changes (configurable) will be coming to this mod:

- When a navigation malfunction occurs, players can purchase a company escort to reroute to their previous moon at a cost (300) - this does not prevent other malfunctions from happening during that moon's stay, nor does it fix the display to indicate weather conditions
- After a navigation malfunction has occurred, players will be able to type `reroute` into the terminal to go to their previous moon without any cost
- When a malfunction is set to occur during a day, there will be a guaranteed `repair bot` item that spawns in the dungeon which can be used near the ship to repair all malfunctions - additionally it can be stored in the ship to use at a later time or sold to the company for about (300) scrap value

## Suggestions ##

Want to suggest new features or tweaks for this mod? Feel free to open up a suggestion issue on the GitHub repository!

Furthermore there is an official mod release thread on the Lethal Company Modding Discord!