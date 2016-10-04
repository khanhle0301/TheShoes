/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop.order_details', ['myshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('order_details', {
            url: "/order_details",
            templateUrl: "/app/components/order_details/orderDetailListView.html",
            controller: "orderDetailListController"
        }).state('add_order_detail', {
            url: "/add_order_detail",
            templateUrl: "/app/components/order_details/orderDetailAddView.html",
            controller: "orderDetailAddController"
        }).state('edit_order_detail', {
            url: "/edit_order_detail/:id",
            templateUrl: "/app/components/order_details/orderDetailEditView.html",
            controller: "orderDetailEditController"
        });
    }
})();