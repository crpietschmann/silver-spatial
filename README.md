# SilverSpatial

This project helps bridge the gap between Silverlight and Geo-Spatial data type (such as SQL Spatial). It implements the Well-Known-Binary (WKB) format for importing/exporting data, and will eventually support a few spatial manipulation/translation functions.

**FYI, this project was migrated here from Codeplex before it shutdown.**

It is still early in the projects development, so all the features that will be included within the project are yet to be defined. The project currently supports importing/exporting data via the [Well-Known-Binary (WKB) format](http://dev.mysql.com/doc/refman/5.0/en/gis-wkb-format.html).

A secondary, but still main, goal of this project is also to work at keeping the overall file size of the assembly (.dll) as small as possible to reduce its footprint within the overall size of the Silverlight applications for which it is used within.

Also, the goal of this project is not to provide a Silverlight based mapping control since both the [DeepEarth](http://deepearth.codeplex.com/) project and [Microsoft's Bing Maps Silverlight Control](http://msdn.microsoft.com/en-us/library/ee681884.aspx) fullfill this very well. Instead this project is meant to be used along-side either of those controls, or some other control, within an application.
Special Thanks

I must give a special thanks to the [SharpMap](http://sharpmap.codeplex.com/) project, from which I was able to integrate the Well-Known-Binary support. Without that project to look at for reference, it would have taken quite a bit longer to implement. So, I must give credit where it is due.