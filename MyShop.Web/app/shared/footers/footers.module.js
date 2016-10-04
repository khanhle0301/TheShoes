/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.footers', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('footers', {
            url: "/footers",
            templateUrl: "/app/shared/footers/footerListView.html",
            controller: "footerListController"
        }).state('footer_add', {
            url: "/footer_add",
            templateUrl: "/app/shared/footers/footerAddView.html",
            controller: "footerAddController"
        }).state('footer_edit', {
            url: "/footer_edit/:id",
            templateUrl: "/app/shared/footers/footerEditView.html",
            controller: "footerEditController"
        });
    }
})();