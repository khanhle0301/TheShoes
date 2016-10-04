/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.orders', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('orders', {
            url: "/orders",
            templateUrl: "/app/components/orders/orderListView.html",
            controller: "orderListController"
        }).state('add_order', {
            url: "/add_order",
            templateUrl: "/app/components/orders/orderAddView.html",
            controller: "orderAddController"
        }).state('edit_order', {
            url: "/edit_order/:id",
            templateUrl: "/app/components/orders/orderEditView.html",
            controller: "orderEditController"
        });
    }
})();