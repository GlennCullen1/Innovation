var multiplier : float = 1.5;
private var cache: float;
private var applied : boolean = false;

function Apply (gscript : GunScript) {
	cache = gscript.fireVolume*(multiplier-1);
	gscript.fireVolume += cache;
}
function Remove (gscript : GunScript) {
	gscript.fireVolume -= cache;
}