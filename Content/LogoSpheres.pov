                                     // Optics Tests. Gustave Granroth. 9/19/2015.
// Use 'Initial_frame=1 Final_Frame=120' in the commad window for animations.

#version 3.7;        
#include "textures.inc" 
#include "colors.inc" 
#include "glass.inc"

#include "stones.inc"

// Using physically-realistic light settings, enabling refraction and reflection, with maximum tracing depth.
global_settings {
  assumed_gamma 1.0
  ambient_light rgb<0.05,0.05,0.05>
}

camera {
  right x*image_width/image_height
  location  <0, 6, 0>
  look_at   <0,0,0>
}

            
// create a regular point light source
light_source {
  0*x                  // light's position (translated below)
  color rgb <0.8,0.8,0.8>    // light's color
  translate <-30, 12, 40>
}       
           
           // Basic reflection facet shape
#declare GGlass=
  material {
    texture{ Chrome_Metal pigment {color rgb<0.1,0.15,0.7> } 
                 normal { facets coords 1.5 scale 0.8}
                 finish { phong 0.6 reflection{ 0.1 } }
               }
    
  } 
           
  // Basic reflection facet shape
#declare GGlass2=
  material {
    texture{ Chrome_Metal pigment {color rgb<0.1,0.8,0.1> } 
                 normal { facets coords 1.5 scale 0.8}
                 finish { phong 0.6 reflection{ 0.1 } }
               }
    
  } 
   
#declare OffsetHeight=1;
         
//Cool sphere lights
light_source {
  0*x
  color rgb <0.2,0.8,0.2>
  // light_source { ...
  // put this inside a light_source to give it a visible appearance
  looks_like { sphere { <0.5, 0.6, 0.2 + OffsetHeight>, 1 material { GGlass2 } } }

}
            
            

//Cool sphere lights
light_source {
  0*x
  color rgb <0.2,0.8,0.2>
  // light_source { ...
  // put this inside a light_source to give it a visible appearance
  looks_like { sphere { <-1.5, 0.6, -1.2 + OffsetHeight>, 1 material { GGlass2 } } }

} 

// parallel world lights
#declare ParallelStrength = 0.01;
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <0, 0, 1>
}
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <0, 0, -1>
}
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <0, 1, 0>
}
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <0, -1, 0>
}
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <1, 0, 0>
}
light_source {
  0*x
  color rgb ParallelStrength
   parallel
   point_at <-1, 0, 0>
}





// Basic cylinders and spheres
sphere {
  <-1, 0.5, 0.2 + OffsetHeight>, 1
  material { GGlass }
}                     
sphere {
  <0, 0.5, -1.2 + OffsetHeight>, 1
  material { GGlass }
}
