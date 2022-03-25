% Create a mask map for Unity HDRP without detail mas by copying the metallic,
% ambient occulusion, NO DETAIL MASK and smoothness into one .png file.
createStackedTexture("grass_02_masktexture.png", ...
                     zeros(2048, 2048, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_02_ao.png", ...
                     zeros(2048, 2048, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_02_smoothness.png");

                 
roughness = imread("brick_4_rough_4k.png");
smoothness = 255 - roughness;
createStackedTexture("brick_4_maskTexture_4k.png", ...
                     "brick_4_spec_4k.png", ... % To leave out just add zeros. 
                     "brick_4_AO_4k.png", ...
                     zeros(4096, 4096, 1, 'uint8'), ... % To leave out just add zeros. 
                     smoothness)
                 
                 
createStackedTexture("grass_03_maskTexture.png", ...
                     zeros(2048, 2048, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_03_ao.png", ...
                     zeros(2048, 2048, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_03_smoothness.png");
                 
                 
createStackedTexture("grass_04_maskTexture.png", ...
                     zeros(4096, 4096, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_04_ao.png", ...
                     zeros(4096, 4096, 1, 'uint8'), ... % To leave out just add zeros. 
                     "grass_04_smoothness.png");
