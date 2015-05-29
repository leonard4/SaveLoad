SaveLoad
==========

SaveLoad is a serialized file saving and loading system for Unity 5.x+ that I put together to show as an example. Basically drop this script onto any game object, something like a Controller. This sample is setup to save text from three inputfields and then load them into three text fields. You can use this for saving any custom variable, struct, or class you want and then reload it.

There are two load functions, and two save functions. First there's an uncompressed load, and save. Second theres a compressed version of the load and save functions that use the DotNetZip library. 


License
=======

DotNetZip is derived in part from ZLIB, the C-language library by Mark
Adler and Jean-loup Gailly .  See the License.ZLIB.txt file included in
the DotNetZip download for details.
