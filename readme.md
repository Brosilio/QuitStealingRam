# Quit Stealing Ram!
This is a rewrite/port of 0x3F's QuitStealingRam.


https://github.com/zeroxthreef/Quit-Stealing-Ram


Are you absolutely tired of inefficient programs stealing all of your ram? I know I am, and I made this out of pure hatred of being stuck in an OOM state because I disabled swapfiles to preserve my SSD.
I was too lazy to implement some sort of config file system but the base is there for one.
Modify the values directly.
Probably should run it as administrator.

## Instructions
```
Config Options:
* LowMemThreshold - The memory threshold to pass before starting the murder spree, in megabytes
* IntervalMs - The interval in miliseconds between checking system memory
* DoubleHomicide - Relentlessly slaughter the process
* Killstreak - The number of processes to kill if possible
* Prolicide - Relentlessly slaughter the children of the process too
* Shitlist - The list of processes that can be killed if they gnarf up too much ram
```

## Adding to the shitlist
This program will only kill programs that are on the shitlist unless DeathTo is set to true.

## Dependencies
* windows probably
* 64 bit unless you change some things

## License
* FEAR Labs. Don't Wank My Dick Clean Off (https://youshouldfear.me/resources/licenses/dontwankmydickcleanoff.txt)