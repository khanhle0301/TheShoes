(function (app) {
    app.controller('footerAddController', footerAddController);

    footerAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];

    function footerAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.footer = {            }
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.AddFooter = AddFooter;
      
        function AddFooter() {         
            apiService.post('api/footer/create', $scope.footer,
                function (result) {
                    notificationService.displaySuccess('Thêm mới thành công.');
                    $state.go('footers');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }
       
    }

})(angular.module('myshop.footers'));