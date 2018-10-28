# the-Saurus

![Logo](Assets/stolenLogo.png)

### TT Hackathon 2018 Project

**"Autocomplete" for [Dynamo](https://github.com/DynamoDS/Dynamo) based on statistical/machine learning models.**

![Basic Workflow](Assets/workflowV1.gif)

## Data Models

[**Training Data**](Data/Output/graphData.csv)

| Node A Name                     | Node B Name                               | Node A ID                        | Node B ID                        |
| ------------------------------- | ----------------------------------------- | -------------------------------- | -------------------------------- |
| DSCore.List.Transpose@var[]..[] | DSCore.List.GetItemAtIndex@var[]..[]\_int | b76189ba8c4a49dd875ddc88e806d5df | a2d2a3d30ff14eaaa120993bca904c53 |

[**Custom Package Data**](Data/Output/packageData.csv)

| GUID                                 | NAME                              | PATH                                                                                                            |
| ------------------------------------ | --------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| ecdb3729-0de2-4c50-bdca-28fe881027a2 | Springs.FamilyInstance.ByGeometry | C:\Users\pmitev\AppData\Roaming\Dynamo\Dynamo Revit\2.1\packages\spring nodes\dyf\FamilyInstance.ByGeometry.dyf |

**"Hyper"-Featurized Data (WIP)**

| Node A Name | Node B Name | # Connections to Unique | # Connections Total | Custom? | Core? | ZT?  |
| ----------- | ----------- | ----------------------- | ------------------- | ------- | ----- | ---- |
| string      | string      | int                     | int                 | bool    | bool  | bool |
