"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

For use with Landmark objects - pass in a list of Landmarks
If hl is true, returns the variances, otherwise the report is printed to console
"""


from Landmark import Landmark
import statistics

FUNCTION = "Variances"
DESCRIPTION = "The variance of each landmark's glances and all landmarks"


def var(lmks, hl):
    if len(lmks) == 0:
        print("No landmarks passed to find variance of")
        return
    lmk_looks = []  # Looks of all landmarks in one list
    lmk_vars = []   # Each landmark's variance
    for lmk in lmks:
        if type(lmk) != Landmark:
            print("Not a landmark: " + str(lmk))
        else:
            for look in lmk.looks:
                lmk_looks.append(look)
            if len(lmk.looks) <= 1:
                # print(lmk.name + " needs more data for variance")
                lmk_vars.append((lmk.name, None))
            else:
                lmk_var = statistics.variance(lmk.looks)
                if not hl:
                    print(f"Standard deviation of {lmk.name}: {lmk_var}")
                else:
                    lmk_vars.append((lmk.name, lmk_var))
    if not hl:
        print("-------------------------------------------------------")
        print(f"Total standard deviation:\t{statistics.variance(lmk_looks)}")
    else:
        return lmk_vars, statistics.variance(lmk_looks)
