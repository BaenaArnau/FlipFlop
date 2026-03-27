<?xml version="1.0" encoding="UTF-8"?>
<tileset version="1.10" tiledversion="1.12.0" name="Bola" tilewidth="32" tileheight="32" tilecount="1" columns="1">
 <image source="../Img/Bola.png" width="32" height="32"/>
 <tile id="0" type="rigidbody">
  <properties>
   <property name="can_sleep" type="bool" value="false"/>
   <property name="collision_layer" value="3"/>
  </properties>
  <objectgroup draworder="index" id="3">
   <object id="4" name="CBola" x="0" y="0" width="32" height="32">
    <properties>
     <property name="collision_layer" value="3"/>
    </properties>
   </object>
  </objectgroup>
 </tile>
</tileset>
