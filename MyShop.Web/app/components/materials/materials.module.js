/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.materials', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('materials', {
            url: "/materials",
            parent: 'base',
            templateUrl: "/app/components/materials/materialListView.html",
            controller: "materialListController"
        })
            .state('material_add', {
                url: "/material_add",
                parent: 'base',
                templateUrl: "/app/components/materials/materialAddView.html",
                controller: "materialAddController"
            })
            .state('material_edit', {
                url: "/material_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/materials/materialEditView.html",
                controller: "materialEditController"
            });
    }
})();