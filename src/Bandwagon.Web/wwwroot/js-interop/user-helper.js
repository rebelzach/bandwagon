
import { user as userClient, } from "https://npm.tfl.dev/@trufflehq/sdk";

export function subscribeUser(obj) {
    userClient.observable.subscribe({
        next: (org) => {
            console.log(org);
            obj.invokeMethodAsync("NextUser", org);
        },
        error: (org) => obj.Error(org),
        complete: () => { }
    });
}

export function subscribeOrgUser(obj) {
    userClient.orgUser.observable.subscribe({
        next: (org) => {
            console.log(org);
            obj.invokeMethodAsync("NextOrgUser", org);
        },
        error: (org) => obj.Error(org),
        complete: () => { }
    });
}
