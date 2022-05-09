using Server;

var server = await ServerFactory.RunWithOptions(new WebApplicationOptions());
server.BlockUntilShutdown();