(function () {
    'use strict';
    var controllerId = 'employee';
    angular.module('app').controller(controllerId, ['common', 'datacontext', '$location', employee]);

    function employee(common, datacontext, $location) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
       
        vm.employeeCount = 0;
        vm.employees = [];
        vm.title = 'Employee';
        vm.gotoEmployee = gotoEmployee;

        activate();

        function activate() {
            var promises = [getEmployees()];
            common.activateController(promises, controllerId)
                .then(function () { log('Activated Employee View'); });
        }

        function getMessageCount() {
            return datacontext.getMessageCount().then(function (data) {
                return vm.messageCount = data;
            });
        }

        function getEmployees() {
            return datacontext.getEmployees().then(function (data) {
                return vm.employees = data.data;
            });
        }

        function gotoEmployee(employee) {
            if (employee && employee.EmployeeID) {
                $location.path('employee/' + employee.EmployeeID);
            }
        }
    }
})();