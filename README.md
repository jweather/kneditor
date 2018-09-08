# k'neditor
K'nex CAD tool in Unity.

Written on Windows 10, no cross-platform testing yet.

# Philosophy
Keyboard-driven, numeric keypad required.  Wanted a way to document some of our bigger projects like the 6-foot tall Ferris Octagon.

# Controls
## Construction Controls
See the green sphere? That's the cursor. Move it, creating K'nex as you go, with the numeric keypad, including the diagonals.
Plus/Minus build in/out respectively.

Change rod length with the number keys from 1=green to 6=gray.

Diagonal rods are one step longer by default. Hit D to toggle this behavior.

Hit BACKSPACE or Ctrl-Z to undo.  Hit DELETE to delete the current connector.
Hold DELETE and press an arrow to delete the connector in that direction.

Holding SHIFT while building keeps the cursor where it is.

Tap M to cycle through symmetry modes, which construct extra parts mirrored/rotated around the symmetry axis 
(always the starting connector for now).

## Camera controls
Drag with the left mouse button to pan, drag with the right mouse button to rotate.  Mouse wheel or CONTROL-Plus/Minus to zoom.

Holding CTRL modifies the arrow keys to control the camera instead, rotating it around the selected piece to edit in a different plane.

## File Operations
Hit S to save, L to load.  These should probably be CTRL-keys instead.

# Bugs
More bugs than features right now.

# Todo
 - clone and shift current layer
 - traversing the graph is duplicating nodes sometimes
 - auto-assign connector types while building
   - construct() is calling autoAssign() but it isn't detecting rods that were just created
 - manual override of connector type and orientation (cycle through possible connectors/orientations only?)
 - 3D connectors
 - rod mesh
 - window loses focus on file load
 - orient orange/ender connectors correctly when rod through center (depends on rod's orientation)

# Asset pipeline
K'nex pieces were designed from scratch in OpenSCAD.

 - OpenSCAD export to STL
 - MeshMixer, Import STL, select all, Edit/Reduce, select Deviation, set Max Deviation to 0.01mm, hit Apply (reduces triangle count)
 - MeshMixer export to OBJ
 - import in Unity, set Normals to Calculate (Import tries to smooth everything and looks bad)