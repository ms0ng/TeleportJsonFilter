# TeleportJsonFilter
A tool for [Sam5440/Genshin_Impact_Teleport](https://github.com/Sam5440/Genshin_Impact_Teleport)

Feature:
- Proccess json files in folder `./json`
- Match regex `"position"\s*:\s*[(.+),(.+),(.+)]}` as a vector
- Move json files that unmatch the regex to `./json_unmatch`
- Move json files that too closed to another point `./json_filted` (≤15m)
