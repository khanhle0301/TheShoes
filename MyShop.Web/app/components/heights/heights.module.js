/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.heights', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('heights', {
            url: "/heights",
            parent: 'base',
            templateUrl: "/app/components/heights/heightListView.html",
            controller: "heightListController"
        })
            .state('height_add', {
                url: "/height_add",
                parent: 'base',
                templateUrl: "/app/components/heights/heightAddView.html",
                controller: "heightAddController"
            })
            .state('height_edit', {
                url: "/height_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/heights/heightEditView.html",
                controller: "heightEditController"
            });
    }
})();