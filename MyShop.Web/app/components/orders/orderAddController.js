(function (app) {
    app.controller('orderAddController', orderAddController);

    orderAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function orderAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.order = {
            CreatedDate: new Date(),
            Status: true
        }
       
        $scope.AddOrder = AddOrder;
        function AddOrder() {
            apiService.post('api/order/create', $scope.order,
                function (result) {
                    notificationService.displaySuccess(result.data.CustomerName + ' đã được thêm mới.');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
    }

})(angular.module('myshop.orders'));