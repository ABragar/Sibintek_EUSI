describe('api/util.decorators.js', function() {

    describe('#debounce', function() {

        var DEFAULT_DEBOUNCE = 250;

        it('Should throw if first argument is not a function', function() {
            expect(function() {
                pbaAPI.debounce({});
            }).to.throw(Error);
        });

        it('Should return a decorated function', function() {
            var initial = function() { };
            var decorated = pbaAPI.debounce(initial);

            expect(decorated).to.not.equal(initial);
            expect(decorated).to.be.a('function');
        });

        it('Should debounce', function(done) {
            var count = 0;
            var debounced = pbaAPI.debounce(function() { count++ });

            debounced();
            debounced();

            expect(count).to.equal(0);

            setTimeout(function() {
                expect(count).to.equal(1);
                done();
            }, DEFAULT_DEBOUNCE * 2);
        });

        it('Should debounce by ~' + DEFAULT_DEBOUNCE + 'ms by default', function(done) {
            var count = 0;
            var debounced = pbaAPI.debounce(function() { count++ });

            debounced();

            setTimeout(function() { debounced() }, DEFAULT_DEBOUNCE * 1.2);
            setTimeout(function() { expect(count).to.equal(2); done(); }, DEFAULT_DEBOUNCE * 2.5);
        });

        it('Should debounce by given `time` option', function(done) {
            var TIME = 100;

            var count = 0;
            var debounced = pbaAPI.debounce(function() { count++ }, { time: TIME });

            debounced();

            setTimeout(function() { debounced() }, TIME * 1.2);
            setTimeout(function() { expect(count).to.equal(2); done(); }, TIME * 2.5);
        });

        it('Should execute function on given `timeout` option', function(done) {
            var TIME = 150;
            var TIMEOUT = 500;

            var count = 0;
            var debounced = pbaAPI.debounce(function() { count++ }, { time: TIME, timeout: TIMEOUT });

            var intervalId = setInterval(function() {
                if (count) {
                    clearTimeout(timeoutId);
                    clearInterval(intervalId);
                    done();
                }

                debounced();
            }, TIME / 3);

            var timeoutId = setTimeout(function() {
                clearInterval(intervalId);
                done(Error('Fail'));
            }, TIMEOUT * 2);
        });

        it('Should correctly execute multiple jobs `in parallel`', function(done) {
            var TIMEOUT = DEFAULT_DEBOUNCE * 2;

            var count1 = 0;
            var count2 = 0;
            var count3 = 0;

            var debounced1 = pbaAPI.debounce(function() { count1++ });
            var debounced2 = pbaAPI.debounce(function() { count2-- });
            var debounced3 = pbaAPI.debounce(function() { count3 = 100 });

            var checked1 = false;
            var checked2 = false;
            var checked3 = false;

            debounced1();
            debounced1();
            debounced2();
            debounced2();
            debounced3();
            debounced1();

            setTimeout(function() {
                expect(count1).to.equal(1);
                if (checked2 && checked3) done();
                else checked1 = true;
            }, TIMEOUT);

            setTimeout(function() {
                expect(count2).to.equal(-1);
                if (checked1 && checked3) done();
                else checked2 = true;
            }, TIMEOUT);

            setTimeout(function() {
                expect(count3).to.equal(100);
                if (checked1 && checked2) done();
                else checked3 = true;
            }, TIMEOUT);
        });

    });

});
