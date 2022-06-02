using Server;

var server = await WebApplicationRunner.RunWithOptions(new WebApplicationOptions {Args = args});
server.BlockUntilShutdown();