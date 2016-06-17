./_Scripts 
contains all controller scripts that run the game and should not be attached to a visible GameObject

./_Scripts/Prefab Script
contains scripts that shuold attach to prefabs that are visible in game.

==GameController==
Should only focus on the overarching logic of this level such as what score we have, what level we are in and what to do after construction and when to lunch construction. 
Should be in scene? Y

Public Variables that should be set:
Max Score: There is a switch(level) statement in the script. Once pass this score it will use the level to determine what function to invoke. Use this to give player powerup after successful construction.
Level: Set to the level number of this level so the script can react accordinly. We might want to abandon this and just allow user to go to construction whenever they want.

==ObjectPooler==
Use to load prefabs into the scene when the scene starts and set them inactive. It also will loop through all the inactive prefabs. It will prevent instantiating and destroying on the fly to boost the performance.
It has public functions to give other scripts the prefabs it is pooling.
Don't instantiate and destroy prefabs, use this instead.

Should be in scene? Y
Public Variables that should be set:
Two lists
The size of the lists should be exactly the same. They are set by the size field.
Elements in Object List are prefabs
Elements in Pool amount are ints
Willgrow is a boolean that determines if the pool runs out of object will the pooler grow the pool by instantiating new objects or simply retruns null.

They will determine how many prefabs will be loaded when the scene is loaded. All of them will be inactive.
You can get those prefab using ObjectPooler.Instance().GetPooledObject(1); where 1 is the index.
After using the prefab, don't destroy just set them inactive. The object pooler will get those that are inactive and pass them to another request.

==ObjScatterer==
Scatters prefabs randomly around the map. You can have multiple scripts of this kind in one scene so that you can scatter different type of items in different area of the map.
Should be in scene? Y

X and Z defines the area on the terain object will be scattered. 
limitY defines the max height after which the object won't spwan. This is here to prevent scaterring objects to unreachable places like a platform on a hill. 
Offsets defines the distance away from surfacewhen a object spawn.

The first list scatterObjIndexList is a list of index of the prefabs that will be scattered. The index should be the same index in the ObjectPooler.
The sceond list shuold have the same size as the first. The correspoding elements will indicate how many prefabs will be scattered.

For example in the ObjectPool if you have a list like this

{0:Pickup, 1:Bullet}

In ObjectScatterer entering
scatterObjIndexList: 0
scatterCount:20

Will scatter 20 of the pick up prefabs across the scene. Because the pickup has the index of 0 in the object pooler.



==ObjectRespawner==
Should be in scene? Y
Use by pickup.cs in prefab scripts folder. After being collected some pickup items might want to respawn randomly else where. 
You need to link one ObjScatterer on this script because scatterer will have info on where to scatter.

==InputController==
Should be in scene? Y
This controller manages the all the behaviour triggered by input that is not process by firstpersoncontroller
AKA non movement related inputs

==References==
Provide references to gameobjects such as Text so that you don't need to find it later.

Prefab Script--------------------------------------
==Bullet==
Use on the prefab bullet. Bullet is the gameobject that will be thrown with catapull. Now it is just a box and you can throw it when pressing x in level 2 after collecting enough score.

==pickup==
script for pickup items.

==Rotate==
Spin objects around

==WorldBox==
for invisble war and will display warning

==Teleport==
Attach to the giant white tower and touch it to go to next level.

Other Notes:
I also modified the script on the RigidBodyFPSController so that the gameController script can adjust the jump force. Using a standard asset firstpersoncontroller will break the functionality.

