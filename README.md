## Documentation

Project documentation is generated with **DocFX** and is available as a static website.

Documentation website:  
[Open UrlShortener Documentation](https://Biomysor.github.io/UrlShortener/)

The documentation contains:

- project overview;
- getting started guide;
- architecture description;
- API documentation generated from XML comments;
- description of main services, handlers, domain models and integration events.

To generate documentation locally:

```bash
dotnet build
docfx docfx.json --serve
