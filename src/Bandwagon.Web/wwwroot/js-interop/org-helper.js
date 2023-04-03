
import { org as orgClient, } from "https://npm.tfl.dev/@trufflehq/sdk";

export function subscribe(obj) {
    orgClient.observable.subscribe({
        next: (org) => {
            console.log(org);
            obj.invokeMethodAsync("NextOrg", org);
        },
        error: (org) => obj.Error(org),
        complete: () => { }
    });
}
