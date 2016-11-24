/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.types', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('types', {
            url: "/types",
            parent: 'base',
            templateUrl: "/app/components/types/typeListView.html",
            controller: "typeListController"
        })
            .state('type_add', {
                url: "/type_add",
                parent: 'base',
                templateUrl: "/app/components/types/typeAddView.html",
                controller: "typeAddController"
            })
            .state('type_edit', {
                url: "/type_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/types/typeEditView.html",
                controller: "typeEditController"
            });
    }
})();