angular.module('SignalR', [])
.constant('$', window.jQuery)
.factory('Hub', ['$', function ($) {
	//This will allow same connection to be used for all Hubs
	//It also keeps connection as singleton.
	var globalConnections = [];

	function initNewConnection(options) {
		var connection = null;
		if (options && options.rootPath) {
			connection = $.hubConnection(options.rootPath, { useDefaultPath: false });
		} else {
			connection = $.hubConnection();
		}

		connection.logging = (options && options.logging ? true : false);
		return connection;
	}

	function getConnection(options) {
		var useSharedConnection = !(options && options.useSharedConnection === false);
		if (useSharedConnection) {
			return typeof globalConnections[options.rootPath] === 'undefined' ?
			globalConnections[options.rootPath] = initNewConnection(options) :
			globalConnections[options.rootPath];
		}
		else {
			return initNewConnection(options);
		}
	}

	return function (hubName, options) {
		var hub = this;

		hub.connection = getConnection(options);
		hub.proxy = hub.connection.createHubProxy(hubName);

		hub.on = function (event, fn) {
			hub.proxy.on(event, fn);
		};
		hub.invoke = function (method, args) {
			return hub.proxy.invoke.apply(hub.proxy, arguments)
		};
		hub.disconnect = function () {
			hub.connection.stop();
		};
		hub.connect = function () {
			return hub.connection.start(options.transport ? { transport: options.transport } : null);
		};

		if (options && options.listeners) {
			angular.forEach(options.listeners, function (fn, event) {
				hub.on(event, fn);
			});
		}
		if (options && options.methods) {
			angular.forEach(options.methods, function (method) {
				hub[method] = function () {
					var args = $.makeArray(arguments);
					args.unshift(method);
					return hub.invoke.apply(hub, args);
				};
			});
		}
		if (options && options.queryParams) {
			hub.connection.qs = options.queryParams;
		}
		if (options && options.errorHandler) {
			hub.connection.error(options.errorHandler);
		}
        //DEPRECATED
		//Allow for the user of the hub to easily implement actions upon disconnected.
		//e.g. : Laptop/PC sleep and reopen, one might want to automatically reconnect 
		//by using the disconnected event on the connection as the starting point.
		if (options && options.hubDisconnected) {
		    hub.connection.disconnected(options.hubDisconnected);
		}
		if (options && options.stateChanged) {
		    hub.connection.stateChanged(options.stateChanged);
		}

		//Adding additional property of promise allows to access it in rest of the application.
		hub.promise = hub.connect();
		return hub;
	};
}]);
