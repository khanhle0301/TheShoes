(function (app) {
    app.controller('contactDetailAddController', contactDetailAddController);

    contactDetailAddController.$inject = ['apiService', '$scope', 'notificationService', '$state'];

    function contactDetailAddController(apiService, $scope, notificationService, $state) {
        $scope.contactDetail = {          
            Status: true
        }
        $scope.ph_numbr = /^(\+?(\d{1}|\d{2}|\d{3})[- ]?)?\d{3}[- ]?\d{3}[- ]?\d{4}$/;
        $scope.AddContactDetail = AddContactDetail;

        function AddContactDetail() {
            apiService.post('api/contactdetail/create', $scope.contactDetail,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('contact_details');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
                });
        }       
    }

})(angular.module('myshop.contact_details'));