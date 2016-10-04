/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('myshop',
        ['myshop.products',
         'myshop.product_categories',
         'myshop.post_categories',
         'myshop.posts',
         'myshop.slides',
         'myshop.pages',
         'myshop.footers',
         'myshop.contact_details',
         'myshop.feedbacks',
         'myshop.orders',
         'myshop.order_details',
         'myshop.banners',
         'myshop.vendors',
         'myshop.common'])
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        $urlRouterProvider.otherwise('/admin');
    }
})();