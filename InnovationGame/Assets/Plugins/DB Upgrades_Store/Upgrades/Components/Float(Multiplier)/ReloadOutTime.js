var multiplier : float = 1.5;
private var cache: float;
private var applied : boolean = false;

function Apply (gscript : GunScript) {
	cache = gscript.reloadOutTime*(multiplier-1);
	gscript.reloadOutTime += cache;
}
function Remove (gscript : GunScript) {
	gscript.reloadOutTime -= cache;
}