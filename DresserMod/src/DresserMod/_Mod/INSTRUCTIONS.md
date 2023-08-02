# Dresser Mod

For an unknow reason I wanted to make the mod a little eccentric.

So here is the vocabulary:

- Cloth: the image and it's variable attach
- Attire: a set of cloth
- Wardrobe: the collection of attires of an object(e.g. Bros, Enemies, block, ect.) I call the wearers


## Add Attires

Place your attires (json file and images) inside `assets` folder.  
It can be in subfolder or not but never forget to put the images at the same place at the json file where they are referenced.

## Create an attire 

First choose the wearers. It can be a bro, an enemy or anything else which is rendered in the game.  
I'll use Rambro as an example.

Next create a json file and we'll have a look of what to write inside.  
First we write the wearer inside like this:
```json
{
	"Wearer": "Rambro"
}
```
You can insert `"Name": "insert name here"`, it is only show on the mod UI. If there is no name it will took the file name.  
Then we need to find which texture can be changed. You can use the software I use while modding which is [DnSpy](https://github.com/dnSpy/dnSpy/releases/tag/v6.1.8).

For bros we have the sprite, the gun sprite, the disarmed sprite, which is show rarely in campaign mode but mainly in deathmatch, the avatar and the ammo icon.  
Each sprite is assigned to a variable. e.g. the basic is sprite is `sprite` ; the gun sprite is `gunSprite` ; disarmed sprite is `disarmedGunMaterial`   
Inside of the json file, add a line, the key is the variable name and the value the image name (Don't forget the file extensions if there is one!).  
E.g.
```json
{
	"Wearer": "Rambro",
	"sprite": "GoodSprite.png",
	"gunSprite": "FancyGun.png"
}
```

The suported variable are only of type `SpriteSM`, `Material`, `Texture` and `Texture2D`. 

However if you need to access a type of a supported variable through another, add `/` between each variable until you reach the desired variable. 

*I'll use the player avatar as example.*  
The avatar is not in the Bro class (there is exception if the bro have more than 2), the avatar is store in the `PlayerHUD` class.  
But how we get to it ? First wee need to go through the `player` variable then `hud` and to finish `avatar`.

So in the Json file it looks like this :
```
{
	"Wearer": "Rambro",
	"player/hud/avatar": "CoolAvatar.png"
}
```



