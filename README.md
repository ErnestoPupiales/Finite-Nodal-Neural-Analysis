# Finite-Nodal-Neural-Analysis
Mobile Finite Nodal Neural Analysis is a virtual laboratory that allows the user to interact with the stress fields over a plate with a hole by applying the boundary conditions using the screen touch in a mobile platform. Artificial Neural Networks were trained to predict the Von Misses and Tresca stress fields and the coordinates of the nodes that discretize the continuous medium to achieve such a goal.

![](images/AppScreenShot.png)

## Releases

For each version you can download the android app instaler:

| **Version** | **Release Date** | **Download** |
|:-------:|:------:|:-------------:|
| **main (unstable)** | -- | -- |
| **v1.0.0** | **April 20, 2021** | **[download](Releases/MFNNAv1.0.0.apk)** |

## Instructions

1. Type the Semi-Major Axis. Neural Networks were trained for values as describe below.
2. Type the Semi-Minor Axis. The value must be equal or minor than the Semi-Major Axis.
3. Type the displacement boundary conditions. Neural Networks were trained for values as describe below.
4. Select the stress field you want to plot
5. Press Solve Button
6. Activate the boundary conditions with the touch input by holding on one border and pulling in the other one.

## Trained values for the inputs

In the case of the Semi Axis, these values are obligatory, otherwise the node cloud is not consistent.
In the case of the Displacement Boundary condition is possible to predict solutions for values higher than the trained ones.

# v 1.0.0

- Semi-Major Axis belongs to (0,0.64) [m]
- Displacement boundary condition belongs to [0.0025,0.05] [mm]

# v 2.0.0 

- Semi-Major Axis belongs to (0,0.64) [m]
- Displacement boundary condition belongs to [0.7,14] [mm]





