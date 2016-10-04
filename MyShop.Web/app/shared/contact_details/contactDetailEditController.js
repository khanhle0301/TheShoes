(function (app) {
    app.controller('contactDetailEditController', contactDetailEditController);

    contactDetailEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];

    function contactDetailEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.contactDetail = {         
            Status: true
        }

        $scope.UpdateContactDetail = UpdateContactDetail;    

        function loadContactDetailDetail() {
            apiService.get('api/contactdetail/getbyid/' + $stateParams.id, null, function (result) {
                $scope.contactDetail = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateContactDetail() {
            apiService.put('api/contactdetail/update', $scope.contactDetail,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('contact_details');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }       
        loadContactDetailDetail();
    }

})(angular.module('myshop.contact_details'));