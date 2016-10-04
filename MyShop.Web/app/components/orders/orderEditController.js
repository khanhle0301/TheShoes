(function (app) {
    app.controller('orderEditController', orderEditController);

    orderEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function orderEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.order = {};
       
        $scope.UpdateOrder = UpdateOrder;

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateOrder() {
            apiService.put('api/order/update', $scope.order,
                function (result) {
                    notificationService.displaySuccess(result.data.CustomerName + ' đã được cập nhật.');
                    $state.go('orders');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }
        loadOrderDetail();
    }

})(angular.module('myshop.orders'));