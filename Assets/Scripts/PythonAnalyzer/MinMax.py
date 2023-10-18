"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

For use with Landmark objects - pass in a list of Landmarks
If hl is true, returns the mins and maxs, otherwise the report is printed to console
"""


from Landmark import Landmark

FUNCTION = "Min/Max Looks"
DESCRIPTION = "Gives the shortest and longest looks of all and each landmarks"


def min_max(lmks, hl):
    if len(lmks) == 0:
        print("No landmarks passed to find mins/maxs of")
        return
    lmk_mins_maxs = []
    tot_min = None
    tot_max = 0
    for lmk in lmks:
        if type(lmk) != Landmark:
            print("Not a landmark: " + str(lmk))
        else:
            lmk_min = min(lmk.looks)
            lmk_max = max(lmk.looks)
            if tot_min is None:
                tot_min = lmk_min
            elif tot_min > lmk_min:
                tot_min = lmk_min
            if tot_max < lmk_max:
                tot_max = lmk_max
            if not hl:
                print(f"Shortest, longest looks at {lmk.name}:\t{lmk_min}, {lmk_max}")
            else:
                lmk_mins_maxs.append((lmk.name, lmk_min, lmk_max))
    if not hl:
        print("-------------------------------------------------------")
        print(f"Shortest, longest looks at any landmark: {tot_min}, {tot_max}")
    else:
        return lmk_mins_maxs, (tot_min, tot_max)
