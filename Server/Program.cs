using Server;

var server = await WebApp.RunWithOptions(new WebApplicationOptions());
server.BlockUntilShutdown();