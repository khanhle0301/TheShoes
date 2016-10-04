/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.vendors', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('vendors', {
            url: "/vendors",
            templateUrl: "/app/components/vendors/vendorListView.html",
            controller: "vendorListController"
        })
            .state('vendor_add', {
                url: "/vendor_add",
                templateUrl: "/app/components/vendors/vendorAddView.html",
                controller: "vendorAddController"
            })
            .state('vendor_edit', {
                url: "/vendor_edit/:id",
                templateUrl: "/app/components/vendors/vendorEditView.html",
                controller: "vendorEditController"
            });
    }
})();