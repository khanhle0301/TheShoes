(function (app) {
    app.controller('footerEditController', footerEditController);

    footerEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];

    function footerEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.footer = {};
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px'
        }
        $scope.UpdateFooter = UpdateFooter;
       
        function loadFooterDetail() {
            apiService.get('api/footer/getbyid/' + $stateParams.id, null, function (result) {
                $scope.footer = result.data;              
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        function UpdateFooter() {           
            apiService.put('api/footer/update', $scope.footer,
                function (result) {
                    notificationService.displaySuccess('Cập nhật thành công.');
                    $state.go('footers');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }       
        loadFooterDetail();
    }

})(angular.module('myshop.footers'));