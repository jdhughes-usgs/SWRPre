# SWRPre

[![Project Status: Inactive – The project has reached a stable, usable state but is no longer being actively developed; support/maintenance will be provided as time allows.](https://www.repostatus.org/badges/latest/inactive.svg)](https://www.repostatus.org/#inactive)

A simple preprocessor for creating MODFLOW-2005 and MODFLOW-NWT SWR Process input. SWRPre creates SWR Process connectivity from an Esri polyline shapefile and a MODFLOW discretization (DIS) file is also included. SWRPre creates a data file that defines SWR Process reaches, reach groups, and reach connectivity. SWRPre also creates 1. an Esri polyline shapefile that contains the data contained in the SWR Process data file to validate the SWR Process network, and 2. an Esri polygon shapefile of the DIS file to validate the coordinate offset and rotation angle provided to the pre-processor.

## Documentation

Documentation of the Surface-Water Routing (SWR1) Process for modeling surface-water flow with the U.S. Geological Survey Modular Ground-Water Model (MODFLOW-2005) is available at [https://pubs.usgs.gov/publication/tm6A40](https://pubs.usgs.gov/publication/tm6A40).
