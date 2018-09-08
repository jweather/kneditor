$fn = 100;
r=19/2;

module socket(shave) {
    add = 0; // additional thickness for 3d printing
    wall = 2 + add*2;
    translate([-2,0,0]) difference() {
        union() {
            translate([(8+2.5)/2,0,0]) cube([8+1,5+2*wall,6.2],true);
            translate([10,add+4.5-0.8,-3.1]) cylinder(r=0.8+add,h=6.2);
            translate([10,-add+-4.5+0.8,-3.1]) cylinder(r=0.8+add,h=6.2);
        }
        translate([0,0,0]) difference() {
            translate([8/2+2.2,0,0]) cube([8.2,5.7,8],true); // minimum diameter
            translate([3+2,-5.7/2,-4]) scale([2,1,1]) cylinder(r=1.2/2, h=8);
            translate([3+2,5.7/2,-4]) scale([2,1,1]) cylinder(r=1.2/2, h=8);
        }
        translate([2/2+2,0,0]) cube([2,6.4,6.2],true); // end cap diameter
        
        // snap
        translate([6.25,0,0]) rotate([0,90,0]) cylinder(r=6.4/2, h=8);
        
        // shave corners
        if (shave) {
            translate([0,5,0]) rotate([0,0,45/2]) cube([8,3,8],true);
            translate([0,-5,0]) rotate([0,0,-45/2]) cube([8,3,8],true);
        }
    }
}

module snowflake() {
    for (a=[0:45:360]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        rotate(a+45/2) translate([6.25,0,0]) cube([5.5,0.95,6.2],true);
    }

    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
}

module yellow() {
    for (a=[0:45:180]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 180) {
            rotate(a+45/2) translate([6.25,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([0,-4,0]) cube([21,1,6.2],true);
}    

module green() {
    for (a=[0:45:135]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 180) {
            rotate(a+45/2) translate([6.25,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([8,-4,0]) cube([16,1,6.2],true);
    translate([0,-4,0]) rotate([0,0,-45]) translate([-11,-1.2,0]) cube([16,1,6.2],true);
}    

module red() {
    for (a=[0:45:90]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 90) {
            rotate(a+45/2) translate([6.25,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([8,-4,0]) cube([16,1,6.2],true);
    translate([0,-4,0]) rotate([0,0,-90]) translate([-11,-4,0]) cube([16,1,6.2],true);
}    

module gray() {
    for (a=[0:45:45]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 45) {
            rotate(a+45/2) translate([6.25,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([8,-4,0]) cube([16,1,6.2],true);
    translate([0,0,0]) rotate([0,0,45]) translate([8,4,0]) cube([16,1,6.2],true);
}    

module orange() {
    for (a=[0:180:180]) {
        rotate(a) translate([r,0,0]) socket(0);
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([0,-4,0]) cube([24,1,6.2],true);
    translate([0,4,0]) cube([24,1,6.2],true);
}    

module ender() {
    translate([r,0,0]) socket(0);
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
    }
    translate([6,-4,0]) cube([12,1,6.2],true);
    translate([6,4,0]) cube([12,1,6.2],true);
}

module purple() {
    for (a=[0:45:135]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 135) {
            rotate(a+45/2) translate([6.45,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
        translate([-10,0,0]) cube([20,20,20],true);
    }
    translate([0,0,0]) cube([1,7,6.2],true); // crossbar
    translate([-4.5,3.85,0]) cube([10,1.3,6.2],true); // inside
    translate([-5,-3.85,0]) cube([10,1.3,6.2],true); // outside
    
    translate([8,-4,0]) cube([16,1,6.2],true);
    translate([0,-4,0]) rotate([0,0,-45]) translate([-16,-1.2,0]) cube([8,1,6.2],true);
    
    translate([-9.4,3.69,-3.1]) cylinder(r=1/2,h=6.2);
}    

module blue() {
    for (a=[225:45:495]) {
        rotate(a) translate([r,0,0]) socket(1);
        
        if (a < 495) {
            rotate(a+45/2) translate([6.45,0,0]) cube([5.5,0.95,6.2],true);
        }
    }
    difference() {
        translate([0,0,-3.1]) cylinder(r=9/2, h=6.2);
        translate([0,0,-4]) cylinder(r=6.4/2, h=8);
        translate([-10,0,0]) cube([20,20,20],true);
    }
    translate([.5,0,0]) cube([1,7,6.2],true); // crossbar
    translate([-4.5,3.85,0]) cube([10,1.3,6.2],true); // inside
    translate([-4.5,-3.85,0]) cube([10,1.3,6.2],true); // outside
    
    translate([0,-4,0]) rotate([0,0,-45]) translate([-16,-1.2,0]) cube([8,1,6.2],true);
    translate([0,4,0]) rotate([0,0,45]) translate([-16,1.2,0]) cube([8,1,6.2],true);
    
    translate([-9.4,3.69,-3.1]) cylinder(r=1/2,h=6.2);
    translate([-9.4,-3.69,-3.1]) cylinder(r=1/2,h=6.2);
}    


*snowflake();
*rotate([0,0,-90]) yellow();
*rotate([0,0,-45]) green();
*red();
rotate([0,0,45]) gray();
*rotate([0,0,90]) orange();
*rotate([0,0,90]) ender();
*purple();
*blue();

*union() {
    blue();
    color([1,0,1]) rotate([90,180,0]) blue();
}

//!socket(1);

