# kneditor
K'nex CAD tool in Unity

# Asset pipeline
K'nex pieces were designed from scratch in OpenSCAD.

 - OpenSCAD export to STL
 - MeshMixer, Import STL, select all, Edit/Reduce, select Deviation, set Max Deviation to 0.01mm, hit Apply (reduces triangle count)
 - export to OBJ
 - import in Unity, set Normals to Calculate (Import tries to smooth everything and looks bad)