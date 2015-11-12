/// <reference path="angular.js" />

var agentsMonitorApp = angular.module('agentsMonitorApp', ["googlechart", "SignalR", "cgBusy"]);

agentsMonitorApp.controller('ServersListController', [
    "$scope", "$filter", "serversService", function($scope, $filter, serversService) {
        $scope.loadServers = function() {
            $scope.serversPromise = serversService.getServers().then(function(servers) {
                $scope.servers = servers;
            });
        };
        $scope.showStatistics = function(serverId) {
            $scope.currentServer = $filter('filter')($scope.servers, { ServerId: serverId }, true)[0];
            $scope.spaceStatisticsPromise = serversService.getStatistics(serverId).then(function(statisticsData) {
                $scope.spaceStatistics = serversService.getSpaceStatisticsChartData(statisticsData);
            });
        };

        $scope.loadServers();

        this.hub = serversService.initializeHub(
            function(data) {
                $scope.$apply(function() {
                    $scope.servers.push(data);
                });
            },
            function(data) {
                if ($scope.currentServer && $scope.currentServer.ServerId === data.ServerId) {
                    $scope.$apply(function() {
                        $scope.spaceStatistics = serversService.getSpaceStatisticsChartData(data);
                    });
                }
            }
        );
    }
]);

agentsMonitorApp.constant("urls", {
    signalR: "/SignalR",
    getSpaceStatistics: "Home/SpaceStatistics",
    getServers: "Home/Servers"
});

agentsMonitorApp.constant("resources", {
    chart: {
        freeSpaceTitle: "Free space (Gb)",
        usedSpaceTitle: "Used space (Gb)"
    }
});

agentsMonitorApp.factory('serversService', [
    "$http", "$q", "Hub", "urls", "resources", function($http, $q, hub, urls, resources) {
        function getStatistics(serverId) {
            var deferred = $q.defer();
            $http({
                url: urls.getSpaceStatistics,
                method: "GET",
                params: { serverId: serverId }
            }).then(function(statistics) {
                deferred.resolve(statistics.data);
            });
            return deferred.promise;
        };

        function getServers() {
            var deferred = $q.defer();
            $http.get(urls.getServers).then(function(servers) {
                deferred.resolve(servers.data);
            });
            return deferred.promise;
        }

        function initializeHub(addServerListener, updateSpaceStatisticsListener) {
            return new hub('ServersHub', {
                rootPath: urls.signalR,
                listeners: {
                    addServer: addServerListener,
                    updateSpaceStatistics: updateSpaceStatisticsListener
                }
            });
        }

        function getSpaceStatisticsChartData(statisticsData) {
            return {
                type: "PieChart",
                data: {
                    cols: [
                        { id: "s", label: "Space", type: "string" },
                        { id: "k", label: "Gb", type: "number" }
                    ],
                    rows: [
                        {
                            c: [
                                { v: resources.chart.freeSpaceTitle },
                                { v: statisticsData.FreeSpace }
                            ]
                        },
                        {
                            c: [
                                { v: resources.chart.usedSpaceTitle },
                                { v: statisticsData.UsedSpace }
                            ]
                        }
                    ]
                },
                formatters: {
                    number: [
                        {
                            columnNum: 1,
                            fractionDigits: 3
                        }
                    ]
                }
            };
        }

        return { getStatistics, getServers, getSpaceStatisticsChartData, initializeHub };
    }
]);