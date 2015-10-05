// Here you can add settings which change per environment. This file will be also used by the buildserver to inject environment specific values.
// Settings are made available through the configService.
var environmentSettings = {
	host: 'localhost',
	port: 8001
}

// Leave lines below untouched. This how gulp file is also able to use this settings file. ('require' needs a module)
if(typeof module !== 'undefined'){
   module.exports = environmentSettings;
};