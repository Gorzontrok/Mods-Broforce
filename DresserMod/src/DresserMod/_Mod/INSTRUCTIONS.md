# Dresser Mod

For an unknow reason I wanted to make the mod a little eccentric.

So here is the vocabulary:

- Cloth: the image and it's variable attach
- Attire: a set of cloth
- Wardrobe: the collection of attires of an object(e.g. Bros, Enemies, block, ect.) I call them wearers


## Add Attires

Place your attires (json file and images) inside `Broforce/DM_Wardrobes/` folder.  
It can be in subfolder or not but never forget to put the images at the same place at the json file where they are referenced.

## Create an attire 

First choose the wearers. It can be a bro, an enemy or anything else which is rendered in the game.  
I'll use Rambro as an example.

Next create a json file and we'll have a look of what to write inside.  
First we write the wearer inside like this:

```json
{
	"Name": "New Rambro",
	"Wearer": "Rambro",
	"Clothes": { },
	"Stats": {
		"speed": 150
	}
}
```

`"Name"`: the name of the Attire
`"Wearer"`: the object that will wear the attire
`"Clothes"`: a list of variables with the image path as value (the path start at the folder where the json is placed)
`"Stats"`: a list of variables to tweak the wearer.  

To find variables, you can use the software I use through modding which is [DnSpy](https://github.com/dnSpy/dnSpy/releases/tag/v6.1.8).

### Clothes
In our example, Rambro is a bro, and Bros have sprites that can be changed:  

- `sprite`: the character sprite  
- `gunSprite`: the sprite of the gun
- `disarmedGunMaterial`: which is show rarely in campaign mode but mainly in deathmatch
- `player.hud.avatar`: the avatar of the bro

Example:

```json
{
	[...]
	"Clothes": {
		"sprite": "newRambro_anim.png",
		"gunSprite": "newRambro_gun_anim.png",
	}
	[...]
}
```

It is also possible to change the projectile and grenade sprite through `"projectile": "newProjectile.png` for the projectile and `"specialGrenade": "newGrenade.png"` for the grenade  

In case you need to access a variable through an other, add a point \(`.`\) between each variable until the desired variable.

I'm going to use the player avatar as an example.  
The avatar is not in the Bro class (there is an exception if the bro has more than 2 avatar), the avatar is store in the `PlayerHUD` class.  
But how we get to it ? First we need to go through the `player` variable then `hud` and to finish `avatar`.

So in the Json file it looks like this :

```json
{
	[...]
	"Clothes": {
		"player.hud.avatar": "CoolAvatar.png"
		[...]
	}
	[...]
}
```

### Stats

Stats works the same way as [Clothes](#clothes) does, instead of an image as a value it's the value type of the variable.
If the field is a integer or a float then the value is going to be decimal, if it's a boolean, the value is true or false.


```json
{
	[...]
	"Stats": {
		"speed": 150,
		"useNewFrames": true
	}
}
```