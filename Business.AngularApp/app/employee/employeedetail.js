(function () {
    'use strict';

    var controllerId = 'employeedetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            employeedetail]);

    function employeedetail($routeParams, $location, $scope, $rootScope, $window,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');


        vm.activate = activate;
        vm.employeeIdParameter = $routeParams.id;
        vm.employees = [];
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.newemployee;
        vm.deleteEmployee = deleteEmployee;
        vm.isSaving = false;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedEmployee()], controllerId)
                                .then(function () { log('Activated Employeedetail View'); });

        }

        function deleteEmployee() {
            return bsDialog.deleteDialog('Employee').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.deleteEmployee(vm.employee.EmployeeID)
                .then(success, failed);

                function success() {
                    gotoEmployees();
                }

                function failed(error) { cancel(); }
            }

        }

       


        function getRequestedEmployee() {
            var val = $routeParams.id;
            if (val === 'new') {
               return vm.newemployee = true;
            }

            return datacontext.getEmployee(val)
            .then(function (data) {
                vm.employee = data.data;
                vm.newemployee = false;
            }, function (error) {
                logError('Unable to get employee ' + val);
                gotoEmployees();
            });
        }



        function cancel() {
                gotoEmployees();
        }

        function gotoEmployees() {
            $location.path('/employees');
        }

        function goBack() { $window.history.back(); }


        function save() {

            return datacontext.getEmployee(vm.employee.EmployeeID)
           .then(function (data) {
               if (data != null)
               { vm.newemployee = false; }
               else
               { vm.newemployee = true; }

               SaveEmployee();
           },
               function (error) {
                   if (error.status == 404) {
                       vm.newemployee = true;
                       SaveEmployee();
                   }
               })
            
        }

        function SaveEmployee() {
            vm.isSaving = true;
            if (vm.newemployee === true) {
                return datacontext.saveEmployee(vm.employee)
                    .then(function (saveResult) {
                        vm.isSaving = false;
                    }, function (error) {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.updateEmployee(vm.employee.EmployeeID, vm.employee)
                           .then(function (saveResult) {
                               vm.isSaving = false;
                           }, function (error) {
                               vm.isSaving = false;
                           })
            }
        }

    }
})();