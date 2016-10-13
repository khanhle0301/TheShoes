/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.pages', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('pages', {
            url: "/pages",
            parent: 'base',
            templateUrl: "/app/shared/pages/pageListView.html",
            controller: "pageListController"
        }).state('page_add', {
            url: "/page_add",
            parent: 'base',
            templateUrl: "/app/shared/pages/pageAddView.html",
            controller: "pageAddController"
        }).state('page_edit', {
            url: "/page_edit/:id",
            parent: 'base',
            templateUrl: "/app/shared/pages/pageEditView.html",
            controller: "pageEditController"
        });
    }
})();