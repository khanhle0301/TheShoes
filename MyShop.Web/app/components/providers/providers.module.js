/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.providers', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('providers', {
            url: "/providers",
            templateUrl: "/app/components/providers/providerListView.html",
            controller: "providerListController"
        })
            .state('provider_add', {
                url: "/provider_add",
                templateUrl: "/app/components/providers/providerAddView.html",
                controller: "providerAddController"
            })
            .state('provider_edit', {
                url: "/provider_edit/:id",
                templateUrl: "/app/components/providers/providerEditView.html",
                controller: "providerEditController"
            });
    }
})();