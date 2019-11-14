# Choropleth Generator

* [Overview](#overview)
* [Build](#build)
* [Infrastructure](#infrastructure)

[![choropleth-generator](https://user-images.githubusercontent.com/14102723/68822657-84ece980-065f-11ea-88c7-f8a22eac8e62.gif)](https://user-images.githubusercontent.com/14102723/68822657-84ece980-065f-11ea-88c7-f8a22eac8e62.gif)

## Overview
[Back to Top](#choropleth-generator)

> This work is based on [Mike Bostock](https://github.com/mbostock/)'s [Command-Line Cartography](https://medium.com/@mbostock/command-line-cartography-part-1-897aa8f8ca2c) articles on Medium.

This repository represents my progress towards automating map-based visualizations for a .NET Core / Angular app stack. It builds on the initial efforts I've made in two other repositories.

[mapping-research](https://github.com/JaimeStill/mapping-research) explored using a Windows command line to generate an [equivalent map of Texas](https://github.com/JaimeStill/mapping-research/blob/master/texas/texas-choropleth.md).

[ps-mapping](https://github.com/JaimeStill/ps-mapping) explored automating the process for any state using any compatible [scheme](https://github.com/d3/d3-scale-chromatic).

## Build
[Back to Top](#choropleth-generator)

In order to build this repository, you will need the following installed:

* [.NET Core SDK](https://dotnet.microsoft.com/download)
* [Node.js - Current](https://nodejs.org/en/)
* [Yarn](https://yarnpkg.com/lang/en/)
* [PowerShell Core](https://github.com/PowerShell/PowerShell#get-powershell)

Additionally, you will need to globally install yarn packages on your machine using the following command:

```
yarn global add shapefile d3 d3-geo-projection d3-scale-chromatic ndjson-cli topojson-client topojson-server topojson-simplify
```

## Infrastructure
[Back to Top](#choropleth-generator)