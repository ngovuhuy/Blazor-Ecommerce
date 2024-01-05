redirectToCheckout = function (sessionId) {
    var stripe = Stripe("pk_test_51OKym2Hs50ILJYN97qcUMGJbBsWRBn6ir0kwPbZi98V1eQD3RJi1B6e2W0ZbtKH5HhLptCjui4VtRMKCbZEj0gHU00ibfvImpC");
    stripe.redirectToCheckout({ sessionId: sessionId });
}